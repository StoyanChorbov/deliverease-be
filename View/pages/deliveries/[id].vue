<script setup lang="ts">
import type {DeliveryDetailsDto} from "~/composables/delivery/types/DeliveryDetailsDto";
import {useApi} from "#build/imports";

const route = useRoute()
const id = route.params.id;

const delivery = ref<DeliveryDetailsDto>({});

const handleLoad = async (id: string) => {
    const res = await useApi<DeliveryDetailsDto>(`/deliveries/${id}`)
        .then(res => {
            delivery.value = res;
        })
        .catch((error) => {
            throw new Error("Error creating delivery: ", error);
        })
};

onMounted(async () => {
    await handleLoad(typeof (id) === "string" ? id : id[0]);
});

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
                        <p>Recipients: {{ delivery.recipients?.join(", ") }}</p>
                        <p>Category: {{ delivery.category }}</p>
                        <p>Description: {{ delivery.description }}</p>
                    </v-card-text>
                </v-card>
            </v-col>
        </v-row>
    </v-container>
</template>