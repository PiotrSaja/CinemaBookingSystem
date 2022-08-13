import axios from 'axios'
import { authService } from './auth'

export function initAxios () {
  axios.interceptors.request.use(async (config) => {
        if (await authService.isUserLoggedIn()) {
          let accessToken = await authService.getAccessToken()
          config.headers.common.Authorization = 'Bearer ' + accessToken
      }
    return config
  })
}
