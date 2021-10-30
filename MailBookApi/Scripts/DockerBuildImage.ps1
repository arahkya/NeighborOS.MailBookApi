Invoke-Expression -Command "docker rm -f MailBookApi"

Invoke-Expression -Command "docker build --tag arahk/neighbor-os-mailbook-api:latest .."