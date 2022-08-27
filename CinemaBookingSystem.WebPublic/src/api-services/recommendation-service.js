import Axios from 'axios'

const RESOURCE_NAME = 'recommendations'

export default {
  getType () {
    return Axios.get(`${RESOURCE_NAME}/type`)
  }
}
