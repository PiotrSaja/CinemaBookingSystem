import Axios from 'axios'

const RESOURCE_NAME = 'https://cinema-booking-system.francecentral.cloudapp.azure.com:44351/api/cinema-halls'

export default {
  getAll () {
    return Axios.get(`${RESOURCE_NAME}`)
  },
  getInCinema (cinemaId) {
    return Axios.get(`${RESOURCE_NAME}/cinema/${cinemaId}`)
  },
  get (id) {
    return Axios.get(`${RESOURCE_NAME}/${id}`)
  },
  create (data) {
    return Axios.post(RESOURCE_NAME, data)
  },
  update (id, data) {
    return Axios.put(`${RESOURCE_NAME}/${id}`, data)
  },
  delete (id) {
    return Axios.delete(`${RESOURCE_NAME}?id=${id}`)
  }
}
