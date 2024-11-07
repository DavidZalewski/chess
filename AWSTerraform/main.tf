provider "aws" {
  region = "us-west-2"  # Choose your preferred region
}

# Security group for EC2
resource "aws_security_group" "jenkins_sg" {
  name        = "jenkins_security_group"
  description = "Security group for Jenkins EC2 instance"

  ingress {
    from_port   = 8080
    to_port     = 8080
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }
  # SSH access
  ingress {
    from_port   = 22
    to_port     = 22
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]  # Ideally restrict this to your IP
  }
  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
}

# EC2 instance for Jenkins
resource "aws_instance" "jenkins" {
  ami           = "ami-0c55b159cbfafe1f0"  # Amazon Linux 2 AMI, adjust as needed
  instance_type = "t2.micro"
  key_name      = "your-key-pair-name"
  vpc_security_group_ids = [aws_security_group.jenkins_sg.id]

  tags = {
    Name = "JenkinsServer"
  }

  # User data script to install Jenkins
  user_data = <<-EOF
              #!/bin/bash
              yum update -y
              amazon-linux-extras install epel -y
              wget -O /etc/yum.repos.d/jenkins.repo \
              https://pkg.jenkins.io/redhat-stable/jenkins.repo
              rpm --import https://pkg.jenkins.io/redhat-stable/jenkins.io.key
              yum upgrade -y
              yum install jenkins java-1.8.0-openjdk-devel -y
              systemctl start jenkins
              EOF
}

# Output the EC2 instance public IP
output "instance_ip_addr" {
  value = aws_instance.jenkins.public_ip
}