<script setup lang="ts">
const props = defineProps<DeliveryRowProps>();

interface DeliveryRowProps {
  deliveryName: string;
  startLocation: string;
  endLocation: string;
}
</script>

<template>
  <v-col>
    <v-card variant="outlined">
      <v-card-title class="text-h3 text-center">{{ deliveryName }}</v-card-title>
      <v-card-text class="text-h5 text-center">
        {{ startLocation }} -> {{ endLocation }}
      </v-card-text>
    </v-card>
  </v-col>
</template>

<style scoped>

</style>