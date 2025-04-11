<script setup lang="ts">
import {ref} from 'vue';
import type {ValidationRule} from "vuetify/framework";

const form = ref<HTMLFormElement>();

const email = ref('');
const emailRules: ValidationRule[] = [
  input => {
    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailPattern.test(input) || 'Invalid email format';
  },
]

const password = ref('');
const passwordRules: ValidationRule[] = [
  (input: String) => {
    const minLength = 8;
    return input.length >= minLength || `Password must be at least ${minLength} characters long`;
  },
]

const submit = () => {
  if (form.value?.validate()) {
    // Perform login action
    console.log('Login successful');
  } else {
    console.log('Login failed');
  }
}

</script>

<template>
  <v-container fluid class="d-flex align-center justify-center h-screen w-25">
    <v-card class="w-100">
      <v-card-title class="text-h4 text-center">Login</v-card-title>
      <v-card-text>
        <v-form class="d-flex flex-column align-center justify-center" ref="form" lazy-validation>
          <v-text-field
              v-model="email"
              :rules="emailRules"
              variant="outlined"
              label="Email"
              class="w-100"
              required
          ></v-text-field>
          <v-text-field
              v-model="password"
              :rules="passwordRules"
              variant="outlined"
              label="Password"
              type="password"
              class="w-100"
              required
          ></v-text-field>
          <p>Don't have an account? <a class="pa-0 ma-0" href="/pages/Register">Register here</a></p>
          <v-btn @click="submit" variant="tonal" rounded="xl" color="primary">Login</v-btn>
        </v-form>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<style scoped>

</style>