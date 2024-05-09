<template>
    <v-menu v-model="isMenuOpen" :close-on-content-click="false">
        <template v-slot:activator="{ props }">
            <v-text-field :label="label"
                          :model-value="formattedDate"
                          :rules="rules"
                          readonly
                          v-bind="props"
                          hide-details></v-text-field>
        </template>
        <v-date-picker v-model="selectedDate" hide-actions title="" :color="color">
            <template v-slot:header>
            </template>
        </v-date-picker>
    </v-menu>
</template>

<script setup>
import { ref, computed, watch, defineProps, defineEmits } from "vue";

const { label, color, modelValue, rules } = defineProps([
  "label",
  "color",
  "modelValue",
  "rules"
]);
const emit = defineEmits(["update:modelValue"]);

const isMenuOpen = ref(false);
const selectedDate = ref(modelValue);

watch(selectedDate, (newValue) => {
    if (newValue) {
        emit('update:modelValue', newValue.toLocaleDateString("es"));
    }
});

const formattedDate = computed(() => {
  return selectedDate.value ? selectedDate.value.toLocaleDateString("es") : "";
});
</script>