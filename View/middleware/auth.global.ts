export default defineNuxtRouteMiddleware((to, from) => {
    const auth = useAuth();
    const allowPaths = ['/', '/login', '/register'];

    console.log(auth.isAuthenticated.value);

    if (!allowPaths.includes(to.path) && !auth.isAuthenticated) {
        return navigateTo('/login');
    }
});