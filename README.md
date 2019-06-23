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

