<script setup lang="ts">

import type {DeliveryRowDto} from "~/composables/delivery/types/DeliveryRowDto";
import {useApi} from "~/composables/useApi";
import type {LocationDto} from "~/composables/delivery/types/LocationDto";
import type {VCombobox} from "vuetify/components";
import { getLocationSuggestions, formatLocations } from "~/composables/delivery/LocationService";
import LocationAutofill from "~/components/delivery/LocationAutofill.vue";

const destinationInput = useTemplateRef<VCombobox | null>("destination-input");

const deliveries = ref<DeliveryRowDto[]>([]);
const startingLocationSuggestions = ref<LocationDto[]>([]);
const destinationSuggestions = ref<LocationDto[]>([]);

const formattedDestinations = computed(() =>
    destinationSuggestions.value = formatLocations(destinationSuggestions.value)
);

const selectedDestination = ref<(LocationDto & {displayText: string}) | null>(null);

const getStartingLocationSuggestions = async (input: string) => {
    if (input.length > 2) {
        const res = await getLocationSuggestions(input);
        startingLocationSuggestions.value = res;
    }
}

const getDestinationSuggestions = async (input: string) => {
    if (input.length > 2) {
        const res = await getLocationSuggestions(input);
        destinationSuggestions.value = res;
    }
}

const getDeliveries = async () => {
    // await useApi<DeliveryRowDto[]>('/deliveries/options')
    //     .then(res => {
    //         deliveries.value = res;
    //     })
    //     .catch((err) => {
    //         console.error('Error fetching deliveries:', err);
    //     });
}

watch(selectedDestination, async (newValue) => {
    if (newValue) {
        await getDeliveries();
    }
});

</script>

<template>
    <v-container>
        <LocationAutofill v-model="selectedDestination" label="Starting Location" />
        <v-row>
            <v-col v-for="delivery in deliveries" :key="delivery.id">
                <v-card>
                    <v-card-title>{{ delivery.name }} ({{ delivery.category }})</v-card-title>
                    <v-card-text>
                        <p>Category: {{ delivery.startingLocationDto?.place }} -> {{ delivery.endingLocationDto?.place }}</p>
                        <LazyNuxtLink :to="`/deliveries/${delivery.id}`">
                            <v-btn color="primary">View Details</v-btn>
                        </LazyNuxtLink>
                    </v-card-text>
                </v-card>
            </v-col>
        </v-row>
    </v-container>
</template>