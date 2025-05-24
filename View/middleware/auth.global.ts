export default defineNuxtRouteMiddleware((to, from) => {
    const auth = useAuth();
    const allowPaths = ['/login', '/register'];

    if (!allowPaths.includes(to.path) && !auth.isAuthenticated.value) {
        return navigateTo('/login');
    }
});