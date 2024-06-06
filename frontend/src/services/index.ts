export async function getPatients() {
  const response = await fetch("https://localhost:44312/api/Patients");

  console.log(response);
  const data = await response.json();
  return data;
}
