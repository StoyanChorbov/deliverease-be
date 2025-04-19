export const useAuth = () => {
    const accessToken = useCookie('accessToken');
    const refreshToken = useCookie('refreshToken');

    const setTokens = (access: string, refresh: string) => {
        accessToken.value = access;
        refreshToken.value = refresh;
    }

    const getAccessToken = () => accessToken.value;
    const getRefreshToken = () => refreshToken.value;

    const clearTokens = () => {
        accessToken.value = null;
        refreshToken.value = null;
    };

    const login = async (username: string, password: string) => {
        const {data, error} = await useFetch<LoginResponse>(`${baseUrl}/users/login`, {
            method: 'POST',
            body: {
                username,
                password
            }
        });

        // TODO: Improve error handling
        if (error.value) throw error.value;

        if (data.value == null) throw new Error('Unable to login');

        console.log(data.value);

        const {accessToken, refreshToken} = data.value;

        setTokens(accessToken, refreshToken);

        console.log(getAccessToken());
        console.log(getRefreshToken());
    }

    const logout = () => {
        clearTokens()
    };

    const isAuthenticated = computed(() => getAccessToken() !== null && getAccessToken() !== undefined);

    return {
        login,
        logout,
        getAccessToken,
        getRefreshToken,
        isAuthenticated,
    }
}

interface LoginResponse {
    accessToken: string;
    refreshToken: string;
}