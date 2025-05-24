<script setup lang="ts">
const auth = useAuth();
const router = useRouter();

const logout = async () => {
	auth.logout();
	await router.push('/login');
};
</script>

<template>
	<header>
		<v-app-bar>
			<v-container class="flex justify-between">
				<v-btn v-if="auth.isAuthenticated.value" to="/">Home</v-btn>
				<v-btn v-if="auth.isAuthenticated.value" to="/deliveries/add"
					>Add delivery</v-btn
				>
				<v-btn v-if="auth.isAuthenticated.value" to="/deliveries/find"
					>Find delivery</v-btn
				>
				<v-btn v-if="!auth.isAuthenticated.value" to="/login">
					Login
				</v-btn>
				<v-btn v-if="!auth.isAuthenticated.value" to="/register">
					Register
				</v-btn>
				<v-btn
					v-if="auth.isAuthenticated.value"
					variant="plain"
					@click="logout"
					>Logout</v-btn
				></v-container
			>
		</v-app-bar>
	</header>
</template>
