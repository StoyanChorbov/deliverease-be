<script setup lang="ts">
const props = defineProps<DeliveriesSectionProps>();

interface DeliveriesSectionProps {
    sectionTitle: string;
}

</script>

<template>
    <v-container>
        <p class="text-h2 font-weight-bold text-center">{{ sectionTitle }}</p>
        <DeliveryRow id="1" deliveryName="Delivery 1" startLocation="Location A" endLocation="Location B"/>
        <DeliveryRow id="2" deliveryName="Delivery 2" startLocation="Location C" endLocation="Location D"/>
    </v-container>
</template>

<style scoped>

</style>