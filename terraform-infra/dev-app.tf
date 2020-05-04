#==============
# EKS Cluster 
#==============

resource "aws_iam_role" "cluster" {
  name = "eks-${local.cluster_name}-role"

assume_role_policy = <<POLICY
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Principal": {
        "Service": "eks.amazonaws.com"
      },
      "Action": "sts:AssumeRole"
    }
  ]
}
POLICY
}

resource "aws_iam_role_policy_attachment" "cluster-AmazonEKSClusterPolicy" {
  policy_arn = "arn:aws:iam::aws:policy/AmazonEKSClusterPolicy"
  role       = aws_iam_role.cluster.name
}

resource "aws_iam_role_policy_attachment" "cluster-AmazonEKSServicePolicy" {
  policy_arn = "arn:aws:iam::aws:policy/AmazonEKSServicePolicy"
  role       = aws_iam_role.cluster.name
}

resource "aws_security_group" "cluster" {
  name        = "eks-${local.cluster_name}-sg"
  description = "Cluster and managed node communication"
  vpc_id      = module.vpc.vpc_id

  ingress {
    from_port = 0
    to_port = 0
    protocol = "-1"
    self = true
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }

  tags = {
    Terraform = "true"
    Environment = "dev"
  }
}


resource "aws_s-ingress-workstation-ecurity_group_rule" "clusterhttps" {
  cidr_blocks       = [""]
  description       = "Allow workstation to communicate with the cluster API Server"
  from_port         = 443
  protocol          = "tcp"
  security_group_id = aws_security_group.cluster.id
  to_port           = 443
  type              = "ingress"
}


resource "aws_eks_cluster" "test" {
  name            = "${local.cluster_name}"
  role_arn        = aws_iam_role.cluster.arn

  vpc_config {
    endpoint_private_access = true
    security_group_ids = ["${aws_security_group.cluster.id}"]
    subnet_ids         = concat(module.vpc.private_subnets, module.vpc.private_subnets)
  }

  

  depends_on = [
    aws_iam_role_policy_attachment.cluster-AmazonEKSClusterPolicy,
    aws_iam_role_policy_attachment.cluster-AmazonEKSServicePolicy,
  ]
}

  output "endpoint" {
  value = "${aws_eks_cluster.test.endpoint}"
}

 output "kubeconfig-certificate-authority-data" {
  value = "${aws_eks_cluster.test.certificate_authority.0.data}"
}


#==============
# EKS Managed Node Group 
#==============

resource "aws_iam_role" "worker" {
  name = "eks-${local.worker_node_name}-role"

  assume_role_policy = jsonencode({
    Statement = [{
      Action = "sts:AssumeRole"
      Effect = "Allow"
      Principal = {
        Service = "ec2.amazonaws.com"
      }
    }]
    Version = "2012-10-17"
  })
}

resource "aws_iam_role_policy_attachment" "worker-AmazonEKSWorkerNodePolicy" {
  policy_arn = "arn:aws:iam::aws:policy/AmazonEKSWorkerNodePolicy"
  role       = aws_iam_role.worker.name
}

resource "aws_iam_role_policy_attachment" "worker-AmazonEKS_CNI_Policy" {
  policy_arn = "arn:aws:iam::aws:policy/AmazonEKS_CNI_Policy"
  role       = aws_iam_role.worker.name
}

resource "aws_iam_role_policy_attachment" "worker-AmazonEC2ContainerRegistryReadOnly" {
  policy_arn = "arn:aws:iam::aws:policy/AmazonEC2ContainerRegistryReadOnly"
  role       = aws_iam_role.worker.name
}

resource "aws_eks_node_group" "worker" {
  cluster_name    = aws_eks_cluster.test.name
  node_group_name = "${local.worker_node_name}"
  node_role_arn   = aws_iam_role.worker.arn
  subnet_ids      = module.vpc.private_subnets

  scaling_config {
    desired_size = 1
    max_size     = 1
    min_size     = 1
  }
  instance_types = ["t2.medium"]

  depends_on = [
    aws_iam_role_policy_attachment.worker-AmazonEKSWorkerNodePolicy,
    aws_iam_role_policy_attachment.worker-AmazonEKS_CNI_Policy,
    aws_iam_role_policy_attachment.worker-AmazonEC2ContainerRegistryReadOnly,
  ]
}