FROM python:3.9-slim

RUN apt-get clean \
    && apt-get -y update

RUN apt-get -y install build-essential

WORKDIR /app

COPY src/requirements.txt requirements.txt
RUN pip install -r requirements.txt

COPY src .

EXPOSE 8000
CMD [ "python", "app.py" ]