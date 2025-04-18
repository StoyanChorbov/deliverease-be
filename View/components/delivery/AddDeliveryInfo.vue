<script setup lang="ts">

import {DeliveryCategory} from "~/composables/delivery/types/DeliveryCategory";
import type {AddDeliveryInfoDto} from "~/composables/delivery/types/AddDeliveryInfoDto";

interface AddDeliveryInfoProps {
    nextSectionHandler: (addDeliveryInfoDto: AddDeliveryInfoDto) => void;
}

const props = defineProps<AddDeliveryInfoProps>();

const packageName = ref("");
const startLocation = ref("");
const endLocation = ref("");
const description = ref("");
const category = ref(DeliveryCategory.Other);
const isFragile = ref(false);

const handleNext = () => {
    const addDeliveryInfoDto: AddDeliveryInfoDto = {
        packageName: packageName.value,
        startLocation: startLocation.value,
        endLocation: endLocation.value,
        description: description.value,
        category: category.value,
        isFragile: isFragile.value,
    };

    // Call the next section handler with the DTO
    props.nextSectionHandler(addDeliveryInfoDto);
};

</script>

<template>
    <v-container class="text-center">
        <h1 class="text-3xl font-bold pb-2">Add delivery</h1>
        <v-form>
            <v-text-field v-model="packageName" label="Package name"/>
            <v-text-field v-model="startLocation" label="Start location"/>
            <v-text-field v-model="endLocation" label="End location"/>
            <v-textarea v-model="description" label="Description"/>
            <v-select label="Category" v-model="category" :items="Object.values(DeliveryCategory)"/>
            <v-checkbox v-model="isFragile" label="Is this package fragile?"/>
            <v-btn icon="mdi-arrow-right" variant="outlined" @click="handleNext"></v-btn>
        </v-form>
    </v-container>
</template>