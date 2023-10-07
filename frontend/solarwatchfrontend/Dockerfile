# Use an official Node.js runtime as the base image
FROM node:current

# Set the working directory in the container
WORKDIR /app

# Copy package.json and package-lock.json to the container
COPY package*.json ./

# Install app dependencies
RUN npm install

# Copy the rest of the application code to the container
COPY . .

# Build the React app
RUN npm run build

# Expose port 3000 for the application
EXPOSE 3000

# Define the command to run when the container starts
CMD ["npm", "start"]