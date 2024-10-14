#MYSQL DOCKER

#docker pull mysql:latest

#Now you must create and start the mysql server contêiner

#docker run --name my_mysql -e MYSQL_ROOT_PASSWORD=your_password -d -p 3306:3306 #mysql:latest

#Check if the conteiner is working!

#Now you can connect to database!

#docker exec -it my_mysql mysql -u root -p





#WEBAPI DOCKER

#In the terminal, navigate to the directory where your Dockerfile is and run the #following command to build the image

#docker build -t name-your-image .

#After the image is successfully built, you can run a container from that image with #the following command

#docker run -d -p 5000:5000 name-your-image

#After running the container, you should be able to access its API at:

#http://localhost:5000/api/v1/wake-up

