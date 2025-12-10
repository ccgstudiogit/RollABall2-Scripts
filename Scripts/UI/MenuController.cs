using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class MenuController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Page startingPage;
    [SerializeField] private Page quitGamePage;
    [SerializeField] private GameObject firstFocusItem;
    [SerializeField] private Canvas rootCanvas;

    private Stack<Page> pageStack = new Stack<Page>();

    private void Start()
    {
        if (rootCanvas == null)
            Debug.LogError("MenuController's rootCanvas null. Please assign a reference.");

        if (firstFocusItem != null)
            EventSystem.current.SetSelectedGameObject(firstFocusItem);

        if (startingPage != null)
            PushPage(startingPage);
    }

    public void OnCancel()
    {
        if (rootCanvas.enabled && rootCanvas.gameObject.activeInHierarchy)
        {
            // Checks if title screen is the only page in the stack, if it is push the quitGamePage
            if (pageStack.Count == 1 && quitGamePage != null)
            {
                if (!IsPageInStack(quitGamePage))
                    PushPage(quitGamePage);
                else
                    PopPage(); // If quitGamePage is already shown, pop it
            }
            else if (pageStack.Count > 1)
                PopPage();
        }
    }

    public bool IsPageInStack(Page page)
    {
        return pageStack.Contains(page);
    }

    public bool IsPageOnTopOfStack(Page page)
    {
        return pageStack.Count > 0 && page == pageStack.Peek();
    }

    public void PushPage(Page page)
    {
        page.Enter(true);

        if (pageStack.Count > 0)
        {
            Page currentPage = pageStack.Peek();

            if (currentPage.exitOnNewPagePush)
                currentPage.Exit(false);
        }

        pageStack.Push(page);
    }

    public void PopPage()
    {
        if (pageStack.Count > 1)
        {
            Page page = pageStack.Pop();
            page.Exit(true);

            Page newCurrentPage = pageStack.Peek();

            if (newCurrentPage.exitOnNewPagePush)
                newCurrentPage.Enter(false);
        }
        else
            Debug.LogWarning("Trying to pop a page but only 1 page remains in the stack!");
    }

    public void PopAllPages()
    {
        for (int i = 1; i < pageStack.Count; i++)
        {
            PopPage();
        }
    }
}
