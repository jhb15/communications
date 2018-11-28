echo "$ENCRYPTED_DOCKER_PASSWORD" | docker login -u "$ENCRYPTED_DOCKER_USERNAME" --password-stdin
cd Comms
docker build -t sem56402018/comms:$1 -t sem56402018/comms:$TRAVIS_COMMIT .
docker push sem56402018/comms:$TRAVIS_COMMIT
docker push sem56402018/comms:$1
