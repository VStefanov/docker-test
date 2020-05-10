resource "aws_security_group" "worker_group_A" {
   name="${local.worker_group_name}-A-sg"
   vpc_id = module.vpc.vpc_id

   ingress {
       from_port = 22
       to_port = 22
       protocol = "tcp"

       cidr_blocks = [
           "10.0.0.0/8",
       ]
   }

   tags = {
    "Name" = "${local.worker_group_name}-A-sg"
   }
}

resource "aws_security_group" "worker_group_B" {
  name="${local.worker_group_name}-B-sg"
  vpc_id      = module.vpc.vpc_id

  ingress {
    from_port = 22
    to_port   = 22
    protocol  = "tcp"

    cidr_blocks = [
      "192.168.0.0/16",
    ]
  }

   tags = {
    "Name" = "${local.worker_group_name}-B-sg"
   }
}


module "eks" {
  source = "terraform-aws-modules/eks/aws"
  cluster_name = local.cluster_name
  subnets = module.vpc.private_subnets

  vpc_id=module.vpc.vpc_id

    worker_groups = [
        {
            name = "${local.worker_group_name}-A"
            instance_type= "t2.small"
            asg_desired_capacity = 1
            additional_security_group_ids = [aws_security_group.worker_group_A.id] 
        },
        {
            name = "${local.worker_group_name}-B"
            instance_type="t2.small"
            asg_desired_capacity = 1
            additional_security_group_ids = [aws_security_group.worker_group_B.id]
        }
    ]
}

data "aws_eks_cluster" "cluster" {
  name = module.eks.cluster_id
}

data "aws_eks_cluster_auth" "cluster" {
  name = module.eks.cluster_id
}
