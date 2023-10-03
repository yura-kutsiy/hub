# Declare stage using linux/amd64 base image
FROM --platform=linux/amd64 python:3.9-slim as stage-amd64
WORKDIR /app
COPY src/requirements.txt /app/requirements.txt
RUN pip install -r requirements.txt --src /usr/local/src
COPY src .


# Declare stage using linux/arm64 base image
FROM --platform=linux/arm64 arm64v8/python:3.9-slim as stage-arm64
WORKDIR /app
COPY src/requirements.txt /app/requirements.txt
RUN pip install -r requirements.txt --src /usr/local/src
COPY src .


# Create the final image
FROM scratch
# Copy files from amd64 and arm64 images based on the host architecture
COPY --from=stage-amd64 / /
COPY --from=stage-arm64 / /
WORKDIR /app
EXPOSE 8000
CMD [ "python", "app.py" ]
