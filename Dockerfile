# Use the official Python image as the base image
FROM python:3.9-slim

# # Install build-essential and clean the package manager cache
# RUN apt-get update && apt-get install -y build-essential && apt-get clean

# Set the working directory in the container
WORKDIR /app

# Create a virtual environment and activate it
RUN python -m venv venv
ENV PATH="/app/venv/bin:$PATH"

# Copy the requirements file and install the dependencies
COPY src/requirements.txt requirements.txt
RUN pip install --no-cache-dir -r requirements.txt

# Copy the source code into the container
COPY src /app

# Expose port 8000 for the Flask application
EXPOSE 8000

# Command to run the Flask application
CMD ["python", "app.py"]
