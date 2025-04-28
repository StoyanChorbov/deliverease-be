<script setup lang="ts">

import type {DeliveryRowDto} from "~/composables/delivery/types/DeliveryRowDto";
import {useApi} from "~/composables/useApi";

const deliveries = ref<DeliveryRowDto[]>([]);

const getData = async () => {
    const res = await useApi<DeliveryRowDto[]>('/deliveries')
        .then(res => {
            deliveries.value = res;
        });
    if (status.value === 'success' && data.value) {
        deliveries.value = data.value;
    } else {
        console.error("Error fetching deliveries:", error.value);
    }
}

onMounted(async () => {
    await getData();
});

</script>

<template>
    <v-container>
        <v-row>
            <v-col v-for="delivery in deliveries" :key="delivery.id">
                <v-card>
                    <v-card-title>{{ delivery.name }} ({{ delivery.category }})</v-card-title>
                    <v-card-text>
                        <p>Category: {{ delivery.startingLocation?.city }} -> {{ delivery.endingLocation?.city }}</p>
                        <LazyNuxtLink :to="`/deliveries/${delivery.id}`">
                            <v-btn color="primary">View Details</v-btn>
                        </LazyNuxtLink>
                    </v-card-text>
                </v-card>
            </v-col>
        </v-row>
    </v-container>
</template>