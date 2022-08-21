import Axios from 'axios'

const RESOURCE_NAME = 'https://cinema-booking-system.francecentral.cloudapp.azure.com:44351/api/hc'

export default {
  get () {
    return Axios.get(`${RESOURCE_NAME}`)
  }
}
