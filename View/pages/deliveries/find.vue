<script setup lang="ts">

import type {DeliveryRowDto} from "~/composables/delivery/types/DeliveryRowDto";
import {useApi} from "~/composables/useApi";
import type {LocationDto} from "~/composables/delivery/types/LocationDto";
import type {VCombobox} from "vuetify/components";
import {$fetch} from "ofetch";
import {type LocationQueryResponseDto, type LocationQueryFeatureDto} from "~/composables/delivery/types/LocationQueryResponseDto";

const destinationInput = useTemplateRef<VCombobox | null>("destination-input");

const deliveries = ref<DeliveryRowDto[]>([]);
const destinations = ref<LocationDto[]>([]);
const formattedDestinations = computed(() =>
    destinations.value.map(destination => {
        let displayText = "";

        if (destination.street != "") {
            if (destination.number != 0) {
                displayText += `${destination.street} ${destination.number}, `;
            } else {
                displayText += `${destination.street}, `;
            }
        }

        if (destination.place != "") {
            displayText += `${destination.place}, `;
        }

        if (destination.region != "") {
            displayText += `${destination.region}`;
        }

        return {
            ...destination,
            displayText: displayText,
        }
    })
);

const runtimeConfig = useRuntimeConfig();
const accessToken = runtimeConfig.public.mapboxKey;

const selectedDestination = ref<(LocationDto & {displayText: string}) | null>(null);

const getLocationSuggestions = async (input: string) => {
        const res = await $fetch<LocationQueryResponseDto>(`https://api.mapbox.com/search/geocode/v6/forward?q=${input}&types=address%2Csecondary_address%2Cstreet%2Cneighborhood%2Cplace%2Cdistrict%2Clocality%2Cpostcode&access_token=${accessToken}`);

        return res.features.map((feature: LocationQueryFeatureDto) => {
            const context = feature.properties.context;
            return {
                place: context.place ? context.place.name : '',
                street: context.adddress ? context.adddress.name : '',
                number: context.adddress ? parseFloat(context.adddress.address_number) : 0,
                region: context.region ? context.region.name : '',
                longitude: feature.geometry.coordinates[0],
                latitude: feature.geometry.coordinates[1],
            }
        });
}

const getStartingLocationSuggestions = async (input: string) => {
    if (input.length > 2) {
        const res = await getLocationSuggestions(input);
        // destinations.value = res;
    }
}

const getDestinationSuggestions = async (input: string) => {
    if (input.length > 2) {
        const res = await getLocationSuggestions(input);
        destinations.value = res;
    }
}

const getDeliveries = async () => {
    await useApi<DeliveryRowDto[]>('/deliveries/options')
        .then(res => {
            deliveries.value = res;
        })
        .catch((err) => {
            console.error('Error fetching deliveries:', err);
        });
}

watch(selectedDestination, async (newValue) => {
    if (newValue) {
        await getDeliveries();
    }
});

</script>

<template>
    <v-container>
        <v-combobox
            label="Destination"
            ref="destinationInput"
            :items="formattedDestinations"
            v-model="selectedDestination"
            item-title="displayText"
            return-object
            @update:search="getDestinationSuggestions"
        ></v-combobox>
        <v-row>
            <v-col v-for="delivery in deliveries" :key="delivery.id">
                <v-card>
                    <v-card-title>{{ delivery.name }} ({{ delivery.category }})</v-card-title>
                    <v-card-text>
                        <p>Category: {{ delivery.startingLocation?.place }} -> {{ delivery.endingLocation?.place }}</p>
                        <LazyNuxtLink :to="`/deliveries/${delivery.id}`">
                            <v-btn color="primary">View Details</v-btn>
                        </LazyNuxtLink>
                    </v-card-text>
                </v-card>
            </v-col>
        </v-row>
    </v-container>
</template>