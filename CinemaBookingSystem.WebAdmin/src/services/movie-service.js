import Axios from 'axios'

const RESOURCE_NAME = 'https://localhost:44334/api/movies'

export default {
  getAll (page, limit) {
    return Axios.get(`${RESOURCE_NAME}?page=${page}&limit=${limit}`)
  },
  get (id) {
    return Axios.get(`${RESOURCE_NAME}/${id}`)
  },
  create (data) {
    return Axios.post(`${RESOURCE_NAME}/omdb`, data)
  },
  delete (id) {
    return Axios.delete(`${RESOURCE_NAME}?id=${id}`)
  }
}
