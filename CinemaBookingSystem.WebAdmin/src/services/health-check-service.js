import Axios from 'axios'

const RESOURCE_NAME = 'https://saja.website:44351/api/hc'

export default {
  get () {
    return Axios.get(`${RESOURCE_NAME}`)
  }
}
