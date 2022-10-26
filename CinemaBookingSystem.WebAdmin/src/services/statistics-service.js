import Axios from 'axios'

const RESOURCE_NAME = 'https://saja.website:44351/api/statistics'

export default {
  get () {
    return Axios.get(`${RESOURCE_NAME}`)
  }
}
