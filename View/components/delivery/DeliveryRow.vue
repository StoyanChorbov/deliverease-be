<script setup lang="ts">
const props = defineProps<DeliveryRowProps>();

interface DeliveryRowProps {
    id: string;
    deliveryName: string;
    startLocation: string;
    endLocation: string;
}
</script>

<template>
    <v-container>
        <v-card variant="outlined">
            <v-card-title class="text-h3 text-center">
                <NuxtLink :to="`/deliveries/${id}`">{{ deliveryName }}</NuxtLink>
            </v-card-title>
            <v-card-text class="text-h5 text-center">
                {{ startLocation }} -> {{ endLocation }}
            </v-card-text>
        </v-card>
    </v-container>
</template>

<style scoped>

</style>