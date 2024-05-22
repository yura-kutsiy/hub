FROM node:20.11.0 AS build
WORKDIR /usr/local/app
COPY . .
RUN npm install
# RUN npm run build
RUN npm install -g @angular/cli@17.0.0
RUN ng build  --configuration=production 


FROM caddy:2.8
COPY --from=build /usr/local/app/dist/ui/ /usr/share/caddy
