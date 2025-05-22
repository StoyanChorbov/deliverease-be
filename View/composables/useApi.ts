import {useAuth} from "~/composables/useAuth";
import {$fetch} from "ofetch";

export const baseUrl = "https://schorbov.eu/api";
// export const baseUrl = "http://localhost:8081";

export const useApi = async <T>(url: string, options: any = {}) => {
    const auth = useAuth();

    options.headers = options.headers || {};

    if (auth.isAuthenticated) {
        options.headers.Authorization = `Bearer ${auth.getAccessToken()}`;
    }

    return $fetch<T>(baseUrl.concat(url), {
        ...options,
        onResponseError: async ({response, request, options}) => {
            if (response.status === 401 && auth.getRefreshToken()) {
                // Refresh token if expired
                const res = await $fetch(`${baseUrl}/users/refresh`, {
                    method: 'POST',
                    body: {
                        refreshToken: auth.getRefreshToken()
                    }
                }).catch((error) => {
                    if (error.response.status === 401) {
                        auth.clearTokens();
                        return;
                    }
                    throw error;
                });

                if (res == null) {
                    auth.clearTokens();
                    return navigateTo("/login");
                }

                if (res?.accessToken && res?.refreshToken) {
                    auth.setTokens(res.accessToken, res.refreshToken);

                    // Retry the original request
                    const opts: object = {
                        ...options,
                        headers: {
                            ...options.headers,
                            Authorization: `Bearer ${res.accessToken}`
                        }
                    }
                    return await $fetch<T>(url, opts);
                } else {
                    auth.clearTokens();
                    return navigateTo("/login");
                }
            }
        },
    });
};