// https://nuxt.com/docs/api/configuration/nuxt-config
import vuetify, {transformAssetUrls} from "vite-plugin-vuetify";

export default defineNuxtConfig({
  compatibilityDate: '2024-11-01',
  devtools: { enabled: true },
  build: {
    transpile: ['vuetify'],
  },
  modules: [
    (_options, nuxt) => {
        // Add a plugin to the build
        nuxt.hooks.hook('vite:extendConfig', (config) => {
          config.plugins = config.plugins || [];
          config.plugins.push(vuetify({ autoImport: true }))
        });
    }
  ],
  vite: {
    vue: {
      template: {
        transformAssetUrls,
      }
    }
  }
})
