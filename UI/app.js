const namespaceList = document.querySelector(".namespace-list");
const divElement = document.getElementById("dashboard-name");

const jsonData = {
  optional_name: "local-develop-cluster",
  namespaces: ["app", "argocd", "tmp"],
};

const namespaces = jsonData.namespaces;
const optionalName = jsonData.optional_name;
divElement.innerHTML = optionalName;

if (namespaces?.length) {
  namespaces.forEach((namespace) => {
    const namespaceItem = document.createElement("div");
    namespaceItem.classList.add("namespace-item");
    namespaceItem.dataset.namespace = namespace;
    namespaceItem.textContent = namespace;
    namespaceList.appendChild(namespaceItem);
  });
}

// Get the namespace-title element
const namespaceTitle = document.getElementById("namespace-title");
// Add an event listener to each namespace-item element
const namespaceItems = document.querySelectorAll(".namespace-item");
if (namespaceItems?.length) {
  namespaceItems.forEach((item) => {
    item.addEventListener("click", () => {
      // Get the namespace from the data-attribute
      const namespace = item.dataset.namespace;
      // Update the namespace-title element with the clicked namespace
      namespaceTitle.textContent = namespace + " ns";
      // Fetch the pods for the clicked namespace
      const apiUrl = `/kuber/${namespace}/pods`;
      fetch(apiUrl)
        .then((response) => response.json())
        .then((data) => {
          // Render the pod list
          const podList = document.querySelector(".pod-list");
          podList.innerHTML = "";
          const podListHeader = document.createElement("h2");
          podListHeader.textContent = "Pods";
          podList.appendChild(podListHeader);
          const table = document.createElement("table");
          const tableHeader = `
              <tr>
                <th>Name</th>
                <th>Status</th>
                <th>Restarts</th>
                <th>Age</th>
              </tr>
            `;
          table.innerHTML = tableHeader;
          if (data?.length) {
            data.forEach((pod) => {
              const ageInSeconds = pod.age;
              let ageString = "";
              if (ageInSeconds < 60) {
                ageString = `${Math.floor(ageInSeconds)} sec`;
              } else if (ageInSeconds < 3600) {
                const minutes = Math.floor(ageInSeconds / 60);
                ageString = `${minutes} min`;
              } else if (ageInSeconds < 86400) {
                const hours = Math.floor(ageInSeconds / 3600);
                const minutes = Math.floor((ageInSeconds % 3600) / 60);
                ageString = `${hours}h ${minutes}min`;
              } else if (ageInSeconds < 604800) {
                const days = Math.floor(ageInSeconds / 86400);
                const hours = Math.floor((ageInSeconds % 86400) / 3600);
                ageString = `${days}d ${hours}h`;
              } else {
                const days = Math.floor(ageInSeconds / 86400);
                const hours = Math.floor((ageInSeconds % 86400) / 3600);
                ageString = `${days}d`;
              }
              const tableRow = `
                <tr>
                  <td>${pod.name}</td>
                  <td>${pod.status}</td>
                  <td>${pod.restarts}</td>
                  <td>${ageString}</td>
                </tr>
              `;
              table.innerHTML += tableRow;
            });
          }

          podList.appendChild(table);
        })
        .catch(() => {
          console.error(
            `Error fetching data for namespace ${namespace}:,
              error`
          );
        });
    });
  });
}