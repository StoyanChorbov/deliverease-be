<script setup lang="ts">

import type {DeliveryRowDto} from "~/composables/delivery/types/DeliveryRowDto";

const deliveries = ref<DeliveryRowDto[]>([]);

onMounted(async () => {
    const {data, status, error} = await useFetch<DeliveryRowDto[]>('/deliveries');
    if (status.value === 'success' && data.value) {
        deliveries.value = data.value;
    } else {
        console.error("Error fetching deliveries:", error.value);
    }
})

</script>

<template>
<p>Awesome</p>
</template>