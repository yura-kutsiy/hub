FROM --platform=$BUILDPLATFORM node:20.11.0 AS build
WORKDIR /usr/local/app
COPY . .
RUN npm install
# RUN npm run build
RUN npm install -g @angular/cli@17.0.0
RUN ng build  --configuration=production 


FROM nginx:1.25.3
COPY --from=build /usr/local/app/dist/ui/ /usr/share/nginx/html
