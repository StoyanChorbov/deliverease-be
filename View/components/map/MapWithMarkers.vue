<script setup lang="ts">
import mapboxgl from "mapbox-gl";
import "mapbox-gl/dist/mapbox-gl.css";

interface MapMarker {
    lng: number;
    lat: number;
    label: string;
}

const runtimeConfig = useRuntimeConfig();
mapboxgl.accessToken = runtimeConfig.public.mapboxKey;

console.log("key: ", mapboxgl.accessToken)

const mapContainer = ref<HTMLDivElement | null>(null);

const markers: MapMarker[] = [
    {lng: 0, lat: 0, label: "Center"},
    {lng: 22, lat: 15, label: "Random place"},
];

onMounted(() => {
    if (!mapContainer.value) return;

    const map = new mapboxgl.Map({
        container: mapContainer.value,
        style: 'mapbox://styles/mapbox/streets-v12',
        center: [11, 7.5],
        zoom: 2,
    });

    map.on("load", () => {
        map.resize();

        markers.forEach(({lng, lat, label}) => {
            const popup = new mapboxgl.Popup({offset: 0}).setText(label);

            new mapboxgl.Marker()
                .setLngLat([lng, lat])
                // .setPopup(popup)
                .addTo(map);
        })
    })
})

</script>

<template>
    <v-container>
        <div ref="mapContainer" class="w-full h-[400px]">
            <!-- Map will be rendered here -->
        </div>
    </v-container>
</template>