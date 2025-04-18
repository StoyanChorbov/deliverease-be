<script setup lang="ts">
import type {DeliveryDetailsDto} from "~/composables/delivery/types/DeliveryDetailsDto";
import {useFetch} from "#build/imports";
import {baseUrl} from "~/composables/RequestUtils";
import {de} from "vuetify/locale";

const route = useRoute()
const id = route.params.id;

const delivery = ref<DeliveryDetailsDto>({});

const handleLoad = async (id: string) => {
    const {data, status, error} = await useFetch<DeliveryDetailsDto>(`${baseUrl}/deliveries/${id}`);
    if (status.value === 'success' && data.value) {
        delivery.value = data.value;
    } else {
        console.error("Error fetching delivery details:", error.value);
    }
}

onMounted(async () => {
    await handleLoad(typeof (id) === "string" ? id : id[0]);
})

</script>

<template>
    <v-container>
        <v-row>
            <LazyMapWithMarkers/>
            <v-col>
                <v-card>
                    <v-card-title>Delivery Details</v-card-title>
                    <v-card-text>
                        <p>Name: {{ delivery.name }}</p>
                        <p>{{ delivery.recipients?.join(", ") }}</p>
                        <p>Category: {{ delivery.category }}</p>
                        <v-textarea readonly>{{ delivery.description }}</v-textarea>
                    </v-card-text>
                </v-card>
            </v-col>
        </v-row>
    </v-container>
</template>