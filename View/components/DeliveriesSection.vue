<script setup lang="ts">
const props = defineProps<DeliveriesSectionProps>();

interface DeliveriesSectionProps {
  sectionTitle: string;
}
</script>

<template>
  <v-col>
    <p class="text-h2 font-weight-bold text-center">{{ sectionTitle }}</p>
    <DeliveryRow deliveryName="Delivery 1" startLocation="Location A" endLocation="Location B"/>
    <DeliveryRow deliveryName="Delivery 2" startLocation="Location C" endLocation="Location D"/>
  </v-col>
</template>

<style scoped>

</style>