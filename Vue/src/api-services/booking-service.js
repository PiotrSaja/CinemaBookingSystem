import Axios from 'axios'

const RESOURCE_NAME = '/bookings'

export default {
  getAll (id) {
    return Axios.get(`${RESOURCE_NAME}/${id}`)
  },
  getUserBooking (id) {
    return Axios.get(`${RESOURCE_NAME}/user/${id}`)
  },
  create (data) {
    return Axios.post(RESOURCE_NAME, data)
  }
}
