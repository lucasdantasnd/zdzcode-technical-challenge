// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: '2026-05-28',
  ssr: false, // Client-side rendering for direct API communication
  runtimeConfig: {
    public: {
      apiBase: process.env.NUXT_PUBLIC_API_BASE || 'http://localhost:5265'
    }
  },
  css: [
    '~/assets/css/main.css'
  ]
})
