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


# Create the final image using a compatible base image
FROM python:3.9-slim
WORKDIR /app
EXPOSE 8000

# Copy files from amd64 and arm64 stages based on the host architecture
COPY --from=stage-amd64 /app /app
COPY --from=stage-arm64 /app /app

CMD [ "python", "app.py" ]