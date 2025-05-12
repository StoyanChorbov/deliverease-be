# Use small image for smaller size
ARG NODE_VERSION=22
FROM node:${NODE_VERSION}-alpine AS build

# Setup environment
#ENV PNPM_HOME="/pnpm"
#ENV PATH="${PNPM_HOME}:${PATH}"
RUN corepack enable

# Copy package config files to the image
WORKDIR /app
COPY View/package.json View/pnpm-lock.yaml ./
RUN pnpm i # Install dependencies

# Copy and build the application
COPY View/. ./
RUN pnpm run build

# Install production dependencies
FROM node:${NODE_VERSION}-alpine
WORKDIR /app
COPY --from=build /app/.output ./

# Build the application
ENV HOST=0.0.0.0
ENV PORT=3000
ENV VITE_MAPBOX_PUBLIC_KEY=pk.eyJ1Ijoic2xhenkiLCJhIjoiY204NjA0NWF1MjY3cTJrc2F2b2NwdWozeCJ9.hnt0ctjDcF1VDr3TGyfldA

# Start the application
EXPOSE 3000
CMD ["node", "/app/server/index.mjs"]