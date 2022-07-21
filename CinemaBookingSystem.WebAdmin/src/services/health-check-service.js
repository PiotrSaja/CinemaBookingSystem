import Axios from 'axios'

const RESOURCE_NAME = 'https://localhost:44334/api/hc'

export default {
  get () {
    return Axios.get(`${RESOURCE_NAME}`)
  }
}
