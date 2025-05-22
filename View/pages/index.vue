<script setup lang="ts">

import DeliveriesSection from "~/components/delivery/DeliveriesSection.vue";
import { type DeliveryRowDto } from '~/composables/delivery/types/DeliveryRowDto';

const toDeliver = ref<DeliveryRowDto[]>([]);
const toReceive = ref<DeliveryRowDto[]>([])

interface CurrentDeliveriesResponseDto {
    toDeliver: DeliveryRowDto[];
    toReceive: DeliveryRowDto[];
}

const getCurrentDeliveries = async () => {
    const token = useAuth().getAccessToken() ?? "";
    await useApi<CurrentDeliveriesResponseDto>(`/deliveries/current`, {
        method: "GET",
        headers: {
            "Authorization": `Bearer ${token}`,
        }
    })
    .then((res) => {
        toDeliver.value= res.toDeliver;
        toReceive.value = res.toReceive;
    })
    .catch((error) => {
        throw new Error("Error creating delivery: ", error);
    });
}

onMounted(async () => {
    await getCurrentDeliveries();
})

</script>

<template>
    <v-row>
        <v-col cols="6">
            <DeliveriesSection :deliveries="toDeliver" section-title="To Deliver"/>
        </v-col>
        <v-col cols="6">
            <DeliveriesSection :deliveries="toReceive" section-title="To Receive"/>
        </v-col>
    </v-row>
</template>

<style scoped>

</style>