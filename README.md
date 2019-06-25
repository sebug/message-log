# Message Log
While I already did an Azure based approach for this, the current project
is to make the message log work on a mini web server / NAS. Since those are
ridiculously powerful now anyway that won't be too much of an issue, but
well, rewrite.

## Infrastructure Requirements
Putting this on OpenMediaVault, so first of all I need to install Docker.

For this, first add the openmediavault-omvextrasorg plugin

Then go to OMV-Extras in the left menu and enable Docker CE

Now we can connect via ssh and add the erasmus-docker gui plugin

	echo "deb https://dl.bintray.com/openmediavault-plugin-developers/erasmus-docker {distribution} {components}" | sudo tee -a /etc/apt/sources.list

	sudo apt-get install docker-ce

	sudo systemctl start docker
	sudo systemctl enable docker

https://www.linuxbabe.com/linux-server/install-docker-on-debian-8-jessie-server

	sudo apt-get install curl
	sudo curl -L https://github.com/docker/compose/releases/download/1.25.0-rc1/docker-compose-`uname -s`-`uname -m` -o /usr/local/bin/docker-compose
	sudo chmod +x /usr/local/bin/docker-compose


## Deploying on OpenMediaVault
Now that all that is installed we can go ahead and deploy it on OpenMediaVault. Create the environment files messagelogclient with MESSAGE_LOG_ConnectionString pointing to host=logdb and a password of your choosing.

In the database.env file just specify that password as POSTGRES_PASSWORD.

Then just

	sudo docker-compose up -d

And there you go.

## Reverse proxying the message log
Now that we have that up and running we want to reverse proxy it with nginx so that we can add an SSL certificate (yes, it would be much nicer to do this end-to-end, but that for later):

	sudo emacs /etc/nginx/sites-available/openmediavault-webgui

And add the server

server {
  listen 81;
  location / {
    proxy_pass http://localhost:9090;
  }
}

And then reload:

	sudo nginx -s reload

## Enabling SSL
We are following the guide here to enable SSL: https://www.digitalocean.com/community/tutorials/how-to-configure-nginx-with-ssl-as-a-reverse-proxy-for-jenkins

	cd /etc/nginx/
	sudo openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout /etc/nginx/cert.key -out /etc/nginx/cert.crt


And then edit openmediavault-webgui again, adding

server {
  listen 82;

  ssl_certificate	/etc/nginx/cert.crt;
  ssl_certificate_key	/etc/nginx/cert.key;

  ssl on;
  ssl_session_cache builtin:1000 shared:SSL:10m;
  ssl_protocols	TLSv1.2;
  ssl_prefer_server_ciphers on;

  access_log            /var/log/nginx/messagelog.access.log;

  location / {
    proxy_pass http://localhost:9090;
  }
}

Finally reload

	sudo nginx -s reload

## Docker images development
Now that we have enabled our OpenMediaVault to handle Docker, we can start actually coding. First, the Postgresql image:

	cd postgresql
	docker build -t messages_postgresql .
	docker run --name messages_1 -e POSTGRES_PASSWORD=yourpassword -p 5432:5432 -d messages_postgresql
	docker run -it --link messages_1:postgres --rm messages_postgresql sh -c 'exec psql -h "$POSTGRES_PORT_5432_TCP_ADDR" -p "$POSTGRES_PORT_5432_TCP_PORT" -U postgres'

## ASP.NET Core side
We configure connection strings etc. using environment variables with the prefix

	MESSAGE_LOG_

Basically the only environment variable that's exposed is ConnectionString. We set this one to allow use inside Docker and it has the format

	export MESSAGE_LOG_ConnectionString="host=localhost;database=postgres;user id=postgres;password=*****;"

for local development.

To build the aspnet side:

	cd client/message-log


