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
            additional_security_group_ids = [aws_security_group.worker_group_A.id, aws_security_group.postgres_redis.id] 
        },
        {
            name = "${local.worker_group_name}-B"
            instance_type="t2.small"
            asg_desired_capacity = 1
            additional_security_group_ids = [aws_security_group.worker_group_B.id, aws_security_group.postgres_redis.id]
        }
    ]
}

resource "aws_db_instance" "postgres_db" {
  allocated_storage    = 10
  storage_type         = "standard"
  engine               = "postgres"
  engine_version       = "12.2"
  instance_class       = "db.t2.micro"
  name                 = "test-database"
  username             = "postgres"
  password             = "postgres_password_1234"
  parameter_group_name = "default.postgres12.2"
  db_subnet_group_name = aws_db_subnet_group.postgres_db.name
  port                 = 5432
  backup_retention_period = 3
  vpc_security_group_ids = [aws_security_group.postgres_redis.id]
}

resource "aws_db_subnet_group" "postgres_db" {
  name       = "postgres-db-subnet-group"
  subnet_ids = ["${module.vpc.private_subnets[2]}"]
}

resource "aws_elasticache_cluster" "redis" {
  cluster_id           = "redis-cluster"
  engine               = "redis"
  node_type            = "cache.m4.large"
  num_cache_nodes      = 1
  parameter_group_name = "default.redis3.2"
  engine_version       = "3.2.10"
  port                 = 6379
  subnet_group_name    = aws_elasticache_subnet_group.redis_cache.name
  security_group_ids   =[aws_security_group.postgres_redis.id]
}

resource "aws_elasticache_subnet_group" "redis_cache" {
  name       = "redis-cache-subnet-group"
  subnet_ids =["${module.vpc.private_subnets[2]}"] 
}

resource "aws_security_group" "postgres_redis" {
  name="postgres-redis-sg"
  vpc_id      = module.vpc.vpc_id

  ingress {
    from_port = 6379
    to_port   = 6379
    protocol  = "tcp"
    self = true
  }

    ingress {
    from_port = 5432
    to_port   = 5432
    protocol  = "tcp"
    self = true
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }

   tags = {
    "Name" = "${local.worker_group_name}-B-sg"
   }
}


data "aws_eks_cluster" "cluster" {
  name = module.eks.cluster_id
}

data "aws_eks_cluster_auth" "cluster" {
  name = module.eks.cluster_id
}
