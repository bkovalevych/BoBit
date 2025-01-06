# Stage 1: Build the Angular app
FROM node:latest as build

WORKDIR /usr/local/app

# Copy package.json and package-lock.json for installing dependencies
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the entire Angular app into the container
COPY ./ /usr/local/app/

# Build the Angular app
RUN npm run build --prod

# Stage 2: Serve the Angular app using Nginx
FROM nginx:latest

# Remove default Nginx static files
RUN rm -rf /usr/share/nginx/html/*

# Copy Angular build files to Nginx
COPY --from=build /usr/local/app/dist/bo-bit-site/browser /usr/share/nginx/html

# Expose port 80
EXPOSE 80

# Start Nginx server
CMD ["nginx", "-g", "daemon off;"]
