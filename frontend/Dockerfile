# Stage 1: Build
FROM node:20-alpine AS builder
WORKDIR /app

# Copy package.json
COPY package*.json ./
RUN npm install

# Copy all files and build Nuxt
COPY . .
RUN npm run build

# Stage 2: Runtime
FROM node:20-alpine AS runner
WORKDIR /app

# Copy only output folder and package.json
COPY --from=builder /app/.output ./.output
COPY --from=builder /app/package.json ./package.json

# Expose port 3000 and bind to all network interfaces
ENV PORT=3000
ENV HOST=0.0.0.0
EXPOSE 3000

CMD ["node", ".output/server/index.mjs"]
