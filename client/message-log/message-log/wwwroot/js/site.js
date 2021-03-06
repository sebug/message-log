﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
(function () {
    function toggleConfirmedLines(show) {
        localStorage.setItem("showConfirmedLines", show.toString());
        const trs = document.querySelectorAll('tr');
        const confirmedTrs = Array.from(trs)
            .filter(tr => tr.querySelector('.approval-yes'));
        console.log('Confirmed trs: ' + confirmedTrs.length);
        if (show) {
            confirmedTrs.forEach(tr => {
                tr.hidden = false;
            });
        } else {
            confirmedTrs.forEach(tr => {
                tr.hidden = true;
            });
        }
    }

    function customizeMessagePage() {
        const enteredOnElement = document.querySelector('#EnteredOn');
        if (enteredOnElement) {
            enteredOnElement.addEventListener('focus', event => {
                if (!event.target.value) {
                    const currentDate = new Date();
                    const year = currentDate.getFullYear();
                    let month = currentDate.getMonth() + 1;
                    if (month.toString().length == 1) {
                        month = '0' + month;
                    }
                    let day = currentDate.getDate();
                    if (day.toString().length == 1) {
                        day = '0' + day;
                    }
                    let hour = currentDate.getHours();
                    if (hour.toString().length == 1) {
                        hour = '0' + hour;
                    }
                    let minute = currentDate.getMinutes();
                    if (minute.toString().length == 1) {
                        minute = '0' + minute;
                    }
                    event.target.value = year + '-' +
                        month + '-' + day + ' ' +
                        hour + ':' + minute;
                }
            });
        }

        const deleteForms = document.querySelectorAll('form.delete-form');
        for (let i = 0; i < deleteForms.length; i += 1) {
            let deleteForm = deleteForms[i];
            deleteForm.addEventListener('submit', function (ev) {
                let confirmResult = confirm('Voulez-vous vraiment enlever ce message?');
                if (!confirmResult) {
                    ev.preventDefault();
                }
            });
        }

        const showConfirmedLinesCheckbox = document.querySelector('.show-confirmed');
        if (showConfirmedLinesCheckbox) {
            let historyShowConfirmedValue = !(localStorage.getItem("showConfirmedLines") === "false");
            showConfirmedLinesCheckbox.checked = historyShowConfirmedValue;
            showConfirmedLinesCheckbox.addEventListener('change', (event) => {
                toggleConfirmedLines(showConfirmedLinesCheckbox.checked);
            });
            toggleConfirmedLines(historyShowConfirmedValue);
        }
    }

    function autoRefreshStream() {
        function getPageContent() {
            return fetch(location.href).then(r => r.text())
                .then(t => {
                    var dp = new DOMParser();
                    return dp.parseFromString(t, 'text/html');
                });
        }

        function extractMessages(domNode) {
            let els = Array.from(domNode.querySelectorAll('main table tbody tr'));
            return els.map(tr => {
                let tds = Array.from(tr.querySelectorAll('td'));
                    return {
                        EnteredOn: tds[0].innerHTML,
                        Sender: tds[1].innerHTML,
                        Recipient: tds[2].innerHTML,
                        MessageText: tds[3].innerHTML,
                        PriorityText: tds[4].innerHTML,
                        ApprovalText: tds[5].innerHTML
                    };
                });
        }

        function messagesMatch(oldMessages, newMessages) {
            if (oldMessages.length !== newMessages.length) {
                return false;
            }
            for (let i = 0; i < oldMessages.length; i += 1) {
                const oldMessage = oldMessages[i];
                const newMessage = newMessages[i];
                if (oldMessage.EnteredOn !== newMessage.EnteredOn ||
                    oldMessage.Sender !== newMessage.Sender ||
                    oldMessage.Recipient !== newMessage.Recipient ||
                    oldMessage.MessageText !== newMessage.MessageText ||
                    oldMessage.PriorityText !== newMessage.PriorityText ||
                    oldMessage.ApprovalText !== newMessage.ApprovalText) {
                    return false;
                }
            }
            return true;
        }

        function approvalClass(txt) {
            if (!txt) {
                return 'approval-not-entered';
            }
            if (txt.indexOf('Partiel') >= 0) {
                return 'approval-partial';
            }
            if (txt.indexOf('Oui') >= 0) {
                return 'approval-yes';
            }
            return 'approval-not-entered';
        }

        function priorityClass(txt) {
            if (!txt) {
                return 'priority-not-entered';
            }
            if (txt.indexOf('Ele') >= 0) {
                return 'priority-elevated';
            } else if (txt.indexOf('Moy') >= 0) {
                return 'priority-medium';
            } else if (txt.indexOf('Faible')) {
                return 'priority-low';
            } else {
                return 'priority-none';
            }
        }

        function messagesToHTML(messages) {
            let result = '';
            for (let message of messages) {
                let line = '<tr>' +
                    '<td>' + message.EnteredOn + '</td>' +
                    '<td>' + message.Sender + '</td>' +
                    '<td>' + message.Recipient + '</td>' +
                    '<td>' + message.MessageText + '</td>' +
                    '<td class="' + priorityClass(message.PriorityText) + '">' + (message.PriorityText || '') + '</td>' +
                    '<td class="' + approvalClass(message.ApprovalText) + '">' + (message.ApprovalText || '') + '</td>' +
                    '</tr>';
                result += line;
            }

            return result;
        }

        let historyShowConfirmedValue = true;
        const showConfirmedLinesCheckbox = document.querySelector('.show-confirmed');
        if (showConfirmedLinesCheckbox) {
            historyShowConfirmedValue = !(localStorage.getItem("showConfirmedLines") === "false");
            showConfirmedLinesCheckbox.checked = historyShowConfirmedValue;
            showConfirmedLinesCheckbox.addEventListener('change', (event) => {
                historyShowConfirmedValue = showConfirmedLinesCheckbox.checked;
                toggleConfirmedLines(showConfirmedLinesCheckbox.checked);
            });
            toggleConfirmedLines(historyShowConfirmedValue);
        }

        function refreshMessages() {
          getPageContent().then(extractMessages).then(latestMessages => {
            const currentMessages = extractMessages(document);
              if (!messagesMatch(currentMessages, latestMessages)) {
                  document.querySelector('main table tbody').innerHTML = messagesToHTML(latestMessages);
                  setTimeout(() => {
                      const trs = document.querySelectorAll('tr');
                      const confirmedTrs = Array.from(trs)
                          .filter(tr => tr.querySelector('.approval-yes'));
                      if (historyShowConfirmedValue) {
                          confirmedTrs.forEach(tr => {
                              tr.hidden = false;
                          });
                      } else {
                          confirmedTrs.forEach(tr => {
                              tr.hidden = true;
                          });
                      }
                  });
            }
          });
        }

        setInterval(refreshMessages, 5 * 1000);
    }

    if (location.href.toLowerCase().indexOf("message") >= 0) {
        customizeMessagePage();
    } else if (location.href.toLowerCase().indexOf("logstream") >= 0) {
        autoRefreshStream();
    }
}());
