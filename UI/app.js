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
                // ... (existing code for formatting ageString)
              }

              const tableRow = `
                <tr>
                  <td>${pod.name}</td>
                  <td>${pod.status}</td>
                  <td>${pod.restarts}</td>
                  <td>${ageString}</td>
                </tr>
                <tr>
                  <td colspan="4">
                      <button class="log-button" data-namespace="${namespace}" data-pod="${pod.name}">logs</button>
                  </td>
                </tr>
              `;
              table.innerHTML += tableRow;
            });
          }

          podList.appendChild(table);

          // Add event listeners to the log buttons
          const logButtons = document.querySelectorAll(".log-button");
          if (logButtons?.length) {
            logButtons.forEach((button) => {
              button.addEventListener("click", () => {
                const clickedNamespace = button.dataset.namespace;
                const clickedPod = button.dataset.pod;

                // Fetch logs for the clicked pod
                const logsApiUrl = `/kuber/${clickedNamespace}/pods/${clickedPod}/logs`;
                fetch(logsApiUrl)
                  .then((logsResponse) => logsResponse.text())
                  .then((logsData) => {
                    // Create and show a log section
                    showLogs(clickedNamespace, clickedPod, logsData);
                  })
                  .catch(() => {
                    console.error(`Error fetching logs for pod ${clickedPod}`);
                  });
              });
            });
          }
        })
        .catch(() => {
          console.error(`Error fetching data for namespace ${namespace}`);
        });
    });
  });
}

// Function to create or update the log section
function showLogs(namespace, pod, logs) {
  // Check if the log section already exists
  const existingLogSection = document.getElementById("log-section");

  // If the log section exists, update its content
  if (existingLogSection) {
      const logHeader = existingLogSection.querySelector("h2");
      const logContent = existingLogSection.querySelector("pre");

      logHeader.textContent = `Logs for ${pod} in namespace ${namespace}`;
      logContent.textContent = logs;
  } else {
      // If the log section doesn't exist, create a new one
      const logSection = document.createElement("div");
      logSection.id = "log-section";
      logSection.classList.add("log-section");

      const logHeader = document.createElement("h2");
      logHeader.textContent = `Logs for ${pod} in namespace ${namespace}`;

      const logContent = document.createElement("pre");
      logContent.textContent = logs;

      logSection.appendChild(logHeader);
      logSection.appendChild(logContent);

      // Append the log section to the body
      document.body.appendChild(logSection);

      // Style the log section (you can customize this further)
      logSection.style.border = "2px solid DodgerBlue";
      logSection.style.margin = "10px";
      logSection.style.padding = "10px";
      logSection.style.backgroundColor = "#f2f2f2";

      // Optionally, add a close button to remove the log section
      const closeButton = document.createElement("button");
      closeButton.textContent = "Close";
      closeButton.addEventListener("click", () => {
          document.body.removeChild(logSection);
      });
      logSection.appendChild(closeButton);
  }
}