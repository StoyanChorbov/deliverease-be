<script setup lang="ts">

import DeliveriesSection from "~/components/delivery/DeliveriesSection.vue";
</script>

<template>
    <v-row>
        <v-col cols="6">
            <DeliveriesSection section-title="Current"/>
        </v-col>
        <v-col cols="6">
            <DeliveriesSection section-title="Past"/>
        </v-col>
    </v-row>
</template>

<style scoped>

</style>